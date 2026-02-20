import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig({
  plugins: [vue()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5080', // La URL de tu backend .NET
        changeOrigin: true,
        secure: false
      }
    }
  },
  build: {
    outDir: '../wwwroot', // Envía el resultado a la carpeta de archivos estáticos de .NET
    emptyOutDir: true
  }
})
