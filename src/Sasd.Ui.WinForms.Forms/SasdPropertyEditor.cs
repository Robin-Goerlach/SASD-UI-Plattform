using System.ComponentModel;

namespace Sasd.Ui.WinForms.Forms;

/// <summary>
/// Provides a searchable property editor on top of the native WinForms <see cref="PropertyGrid"/>.
/// </summary>
/// <remarks>
/// <para>
/// The editor never owns the selected application object. It creates a lightweight type-descriptor
/// view around that object so filtering and a forced read-only mode can be applied without changing
/// attributes or metadata on the domain type itself.
/// </para>
/// <para>
/// Filtering is intentionally simple in the first R2 implementation: display name, property name,
/// category and description are matched using the current UI culture. This keeps the component
/// predictable and useful without introducing a separate search/indexing subsystem.
/// </para>
/// </remarks>
public sealed class SasdPropertyEditor : UserControl
{
    private readonly TextBox filterTextBox;
    private readonly Button clearFilterButton;
    private readonly PropertyGrid propertyGrid;
    private object? selectedObject;
    private bool forceReadOnly;

    /// <summary>Initialises a property editor with a search field and native property grid.</summary>
    public SasdPropertyEditor()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AccessibleName = "Property editor";

        filterTextBox = new TextBox
        {
            AccessibleName = "Filter properties",
            Dock = DockStyle.Fill,
            PlaceholderText = "Filter properties...",
        };
        filterTextBox.TextChanged += OnFilterTextChanged;

        clearFilterButton = new Button
        {
            AccessibleName = "Clear property filter",
            AutoSize = true,
            Text = "Clear",
        };
        clearFilterButton.Click += OnClearFilterClick;

        var filterLayout = new TableLayoutPanel
        {
            AutoSize = true,
            ColumnCount = 2,
            Dock = DockStyle.Top,
            Margin = Padding.Empty,
            RowCount = 1,
        };
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        filterLayout.Controls.Add(filterTextBox, 0, 0);
        filterLayout.Controls.Add(clearFilterButton, 1, 0);

        propertyGrid = new PropertyGrid
        {
            Dock = DockStyle.Fill,
            HelpVisible = true,
            PropertySort = PropertySort.CategorizedAlphabetical,
            ToolbarVisible = true,
        };

        Controls.Add(propertyGrid);
        Controls.Add(filterLayout);
        MinimumSize = new Size(240, 180);
    }

    /// <summary>
    /// Gets or sets the application-owned object displayed in the editor.
    /// </summary>
    /// <remarks>The editor does not dispose or otherwise take ownership of this object.</remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object? SelectedObject
    {
        get => selectedObject;
        set
        {
            if (ReferenceEquals(selectedObject, value))
            {
                return;
            }

            selectedObject = value;
            RefreshProjection();
        }
    }

    /// <summary>Gets or sets the text used to filter the visible property descriptors.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FilterText
    {
        get => filterTextBox.Text;
        set => filterTextBox.Text = value ?? string.Empty;
    }

    /// <summary>
    /// Gets or sets whether all visible properties are presented as read-only regardless of their
    /// original descriptor metadata.
    /// </summary>
    [Category("SASD")]
    [DefaultValue(false)]
    public bool ForceReadOnly
    {
        get => forceReadOnly;
        set
        {
            if (forceReadOnly == value)
            {
                return;
            }

            forceReadOnly = value;
            RefreshProjection();
        }
    }

    /// <summary>Gets or sets whether the native PropertyGrid toolbar is visible.</summary>
    [Category("SASD")]
    [DefaultValue(true)]
    public bool ToolbarVisible
    {
        get => propertyGrid.ToolbarVisible;
        set => propertyGrid.ToolbarVisible = value;
    }

    /// <summary>Gets or sets whether the native PropertyGrid help panel is visible.</summary>
    [Category("SASD")]
    [DefaultValue(true)]
    public bool HelpVisible
    {
        get => propertyGrid.HelpVisible;
        set => propertyGrid.HelpVisible = value;
    }

    /// <summary>Clears the active property filter.</summary>
    public void ClearFilter() => filterTextBox.Clear();

    /// <summary>
    /// Requests a descriptor refresh after the selected object's property metadata or values changed.
    /// </summary>
    public void RefreshProperties()
    {
        if (selectedObject is not null)
        {
            TypeDescriptor.Refresh(selectedObject);
        }

        RefreshProjection();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            filterTextBox.TextChanged -= OnFilterTextChanged;
            clearFilterButton.Click -= OnClearFilterClick;
            propertyGrid.SelectedObject = null;
            selectedObject = null;
        }

        base.Dispose(disposing);
    }

    private void OnFilterTextChanged(object? sender, EventArgs e) => RefreshProjection();

    private void OnClearFilterClick(object? sender, EventArgs e) => ClearFilter();

    private void RefreshProjection()
    {
        if (selectedObject is null)
        {
            propertyGrid.SelectedObject = null;
            return;
        }

        // PropertyGrid asks the selected object for an ICustomTypeDescriptor. Wrapping the
        // application object lets us alter only the descriptor view; the object itself remains
        // untouched and can continue to be used normally by the application.
        propertyGrid.SelectedObject = new SasdPropertyObjectView(selectedObject, FilterText, forceReadOnly);
        propertyGrid.Refresh();
    }

    private sealed class SasdPropertyObjectView : ICustomTypeDescriptor
    {
        private readonly object component;
        private readonly ICustomTypeDescriptor descriptor;
        private readonly string filterText;
        private readonly bool forceReadOnly;

        public SasdPropertyObjectView(object component, string filterText, bool forceReadOnly)
        {
            this.component = component;
            descriptor = TypeDescriptor.GetProvider(component).GetTypeDescriptor(component);
            this.filterText = filterText.Trim();
            this.forceReadOnly = forceReadOnly;
        }

        public AttributeCollection GetAttributes() => descriptor.GetAttributes();
        public string? GetClassName() => descriptor.GetClassName();
        public string? GetComponentName() => descriptor.GetComponentName();
        public TypeConverter GetConverter() => descriptor.GetConverter();
        public EventDescriptor? GetDefaultEvent() => descriptor.GetDefaultEvent();
        public PropertyDescriptor? GetDefaultProperty() => descriptor.GetDefaultProperty();
        public object? GetEditor(Type editorBaseType) => descriptor.GetEditor(editorBaseType);
        public EventDescriptorCollection GetEvents() => descriptor.GetEvents();
        public EventDescriptorCollection GetEvents(Attribute[]? attributes) => descriptor.GetEvents(attributes);
        public object GetPropertyOwner(PropertyDescriptor? propertyDescriptor) => component;
        public PropertyDescriptorCollection GetProperties() => CreateProperties(descriptor.GetProperties());
        public PropertyDescriptorCollection GetProperties(Attribute[]? attributes) => CreateProperties(descriptor.GetProperties(attributes));

        private PropertyDescriptorCollection CreateProperties(PropertyDescriptorCollection source)
        {
            IEnumerable<PropertyDescriptor> properties = source.Cast<PropertyDescriptor>();
            if (filterText.Length > 0)
            {
                properties = properties.Where(MatchesFilter);
            }

            if (forceReadOnly)
            {
                properties = properties.Select(property => property.IsReadOnly
                    ? property
                    : new SasdReadOnlyPropertyDescriptor(property));
            }

            return new PropertyDescriptorCollection(properties.ToArray(), readOnly: true);
        }

        private bool MatchesFilter(PropertyDescriptor property) =>
            Contains(property.DisplayName, filterText) ||
            Contains(property.Name, filterText) ||
            Contains(property.Category, filterText) ||
            Contains(property.Description, filterText);

        private static bool Contains(string? value, string filter) =>
            !string.IsNullOrEmpty(value) && value.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
    }

    private sealed class SasdReadOnlyPropertyDescriptor : PropertyDescriptor
    {
        private readonly PropertyDescriptor inner;

        public SasdReadOnlyPropertyDescriptor(PropertyDescriptor inner)
            : base(inner)
        {
            this.inner = inner;
        }

        public override Type ComponentType => inner.ComponentType;
        public override bool IsReadOnly => true;
        public override Type PropertyType => inner.PropertyType;
        public override bool CanResetValue(object component) => false;
        public override object? GetValue(object? component) => inner.GetValue(component);
        public override void ResetValue(object component) { }
        public override void SetValue(object? component, object? value) { }
        public override bool ShouldSerializeValue(object component) => inner.ShouldSerializeValue(component);
    }
}
