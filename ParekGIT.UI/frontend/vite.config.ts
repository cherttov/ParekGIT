import { svelte } from '@sveltejs/vite-plugin-svelte'
import { defineConfig } from 'vite'

export default defineConfig({
	plugins: [svelte()],
	base: './',
	build: {
		outDir: '../wwwroot',
		emptyOutDir: true,
		rollupOptions: {
			output: {
				entryFileNames: 'script.js',
				chunkFileNames: '[name].js',
				assetFileNames: (assetInfo) => {
					if (assetInfo.name && assetInfo.name.endsWith('.css')) {
						return 'styles.css';
					}
					return '[name].[ext]';
				}
			}
		}
	}
})
