# Barrierefreiheit, Tastatur und DPI – Checkliste

## 1. Tastatur

- [ ] alle interaktiven Elemente per Tab erreichbar;
- [ ] Tab-Reihenfolge entspricht visueller/fachlicher Reihenfolge;
- [ ] Fokus ist in Light, Dark und High Contrast sichtbar;
- [ ] Enter/Escape-Verhalten in Dialogen korrekt;
- [ ] Shortcuts kollidieren nicht;
- [ ] Kontextaktionen besitzen Tastaturalternative;
- [ ] Fokus kehrt nach Dialog/Overlay sinnvoll zurück;
- [ ] keine Tastaturfalle außer bewusstem modalem Busy-Zustand mit Abbruch.

## 2. Accessibility

- [ ] sinnvoller AccessibleName;
- [ ] Role/ControlType passend;
- [ ] Value/State verfügbar;
- [ ] Fehler nicht nur farblich angezeigt;
- [ ] Icons besitzen Textalternative, wenn Bedeutung nicht redundant ist;
- [ ] Labels sind Eingabefeldern zugeordnet;
- [ ] Validation Summary kann zum Fehlerfeld navigieren;
- [ ] dynamische Statusänderungen werden geeignet bekanntgegeben;
- [ ] AutomationIds stabil und dokumentiert.

## 3. Farben und Kontrast

- [ ] Status nicht ausschließlich Farbe;
- [ ] Text-/Hintergrundkontrast ausreichend;
- [ ] Disabled-Zustand noch erkennbar;
- [ ] Fokusrahmen nicht durch Theme verborgen;
- [ ] High Contrast respektiert Systemfarben oder besitzt sicheren Fallback.

## 4. DPI und Layout

- [ ] 100/125/150/200 Prozent geprüft;
- [ ] Per-Monitor-Wechsel geprüft;
- [ ] Mindestgrößen sinnvoll;
- [ ] Texte nicht abgeschnitten;
- [ ] Icons nicht unscharf oder falsch skaliert;
- [ ] Dialogbuttons vollständig sichtbar;
- [ ] Scrollen statt Überlappung bei kleinen Fenstern;
- [ ] gespeicherte Fensterposition auf aktuelle Monitore validiert.

## 5. Lokalisierung

- [ ] Deutsch und Englisch;
- [ ] verlängerte/pseudolokalisierte Texte;
- [ ] Datums-/Zahl-/Währungsformat;
- [ ] keine fest verdrahteten sichtbaren Strings in wiederverwendbaren Controls;
- [ ] Tooltips und Fehlermeldungen lokalisiert.

## 6. Prüfwerkzeuge

- UI Automation/FlaUI;
- Accessibility Insights for Windows;
- Component Gallery;
- Screenshotmatrix;
- manuelle Tastatur-Only-Prüfung;
- Windows-Hochkontrast und unterschiedliche Skalierungsstufen.

## 7. Abweichungen

Eine Abweichung dokumentiert Komponente, Zustand, Nutzerwirkung, Workaround, Zielrelease und Freigabeentscheidung. Kritische Tastatur- oder Fokusprobleme blockieren ein R1-Release.
