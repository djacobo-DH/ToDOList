// Usamos la nueva integración PostCSS para Tailwind (paquete @tailwindcss/postcss)
module.exports = {
  plugins: [
    require('@tailwindcss/postcss'),
    require('autoprefixer'),
  ],
};
