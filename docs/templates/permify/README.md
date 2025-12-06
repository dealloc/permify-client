# Permify Custom DocFx Template

This custom DocFx template provides a modern, Permify-inspired design for your documentation.

## Features

- **Purple Brand Theme**: Inspired by Permify's brand colors (#8246FF)
- **Dark Mode First**: Beautiful dark theme with light mode support
- **Modern Design**: Clean, professional look with smooth transitions
- **Responsive**: Works great on desktop, tablet, and mobile
- **Enhanced Typography**: Improved readability with gradient headings
- **Code Highlighting**: Better syntax highlighting for code blocks
- **Custom Components**: Styled tables, alerts, buttons, and more

## Color Scheme

The template uses the following color palette:

- **Primary**: #8246FF (Permify Purple)
- **Primary Light**: #9B6BFF
- **Primary Dark**: #6318FF
- **Dark Background**: #0b0a10
- **Secondary Background**: #1a1825
- **Tertiary Background**: #252238

## Customization

You can further customize the theme by editing:

- `public/main.css` - Main stylesheet with all custom styles
- CSS variables in `:root` - Easily adjust colors, spacing, fonts, etc.

## Dark/Light Mode

The template automatically supports both dark and light modes. Users can toggle between themes, and the preference is saved.

## Usage

The template is already configured in your `docfx.json`. To build the documentation:

```bash
docfx build
```

To serve locally:

```bash
docfx serve _site
```

## Template Structure

```
templates/permify/
├── public/
│   └── main.css       # Main custom stylesheet
├── partials/          # Custom template partials (optional)
└── README.md          # This file
```

## Browser Support

- Chrome/Edge (latest)
- Firefox (latest)
- Safari (latest)
- Mobile browsers (iOS Safari, Chrome Mobile)

## Credits

Inspired by [Permify's Documentation](https://docs.permify.co)
