import {fontFamily} from 'tailwindcss/defaultTheme';

/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: ['class'],
  content: [
    './Modules/**/*.razor',
    './Modules.Ui/**/*.razor',
    './ployd/**/*.razor',
  ],
  safelist: ['dark'],
  theme: {
    extend: {
      colors: {
        background: 'var(--background)',
        surface: 'var(--surface)',
        "surface-light": 'var(--surface-light)',
        overlay: 'color-mix(in srgb, var(--primary) 10%, transparent)',
        primary: {
          DEFAULT: 'var(--primary)',
          50: {
            DEFAULT: 'color-mix(in srgb, var(--primary) 10%, white)',
            foreground: 'var(--font-dark)'
          },
          100: {
            DEFAULT: 'color-mix(in srgb, var(--primary) 31%, white)',
            foreground: 'var(--p--font-dark)'
          },
          200: {
            DEFAULT: 'color-mix(in srgb, var(--primary) 46%, white)',
            foreground: 'var(--font-dark)'
          },
          300: {
            DEFAULT: 'color-mix(in srgb, var(--primary) 67%, white)',
            foreground: 'var(--font-dark)'
          },
          400: {
            DEFAULT: 'color-mix(in srgb, var(--primary) 80%, white)',
            foreground: 'var(--font-dark)'
          },
          500: {
            DEFAULT: 'var(--primary)',
            foreground: 'var(--font-light)'
          },
          600: {
            DEFAULT: 'color-mix(in srgb, var(--primary), black 9%)',
            foreground: 'var(--font-light)'
          },
          700: {
            DEFAULT: 'color-mix(in srgb, var(--primary), black 29%)',
            foreground: 'var(--font-light)'
          },
          800: {
            DEFAULT: 'color-mix(in srgb, var(--primary), black 45%)',
            foreground: 'var(--font-light)'
          },
          900: {
            DEFAULT: 'color-mix(in srgb, var(--primary), black 50%)',
            foreground: 'var(--font-light)'
          }
        },
      },
      borderRadius: {
        DEFAULT: 'var(--radius)',
        sm: 'calc(var(--radius) - 2px)',
        md: 'var(--radius)',
        lg: 'calc(var(--radius) + 2px)'
      },
      fontFamily: {
        sans: ['Inter', ...fontFamily.sans]
      }
    }
  },
  plugins: []
}

