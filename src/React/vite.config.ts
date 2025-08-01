import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// Use the PORT environment variable if set, otherwise default to 52879
const port = process.env.PORT ? parseInt(process.env.PORT, 10) : 52879;

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port,
    }
})
