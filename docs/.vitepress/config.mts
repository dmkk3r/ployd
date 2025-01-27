import { defineConfig } from 'vitepress'

// https://vitepress.dev/reference/site-config
export default defineConfig({
  title: "ployd",
  description: "Selfhostable deployment plattform.",
  head: [
		['link', { rel: 'icon', type: 'image/png', href: '/favicon.png' }],
    ['meta', { property: 'og:type', content: 'website' }],
		['meta', { property: 'og:site_name', content: 'ployd' }],
		['meta', { property: 'og:title', content: 'ployd | Selfhostable deployment plattform' }],
		['meta', { property: 'og:url', content: 'https://ployd.dev' }],
		//['meta', { property: 'og:image', content: '' }],
	],
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: [
      { text: 'Home', link: '/' }
    ],

    sidebar: [
      {
        text: 'Examples',
        items: [
          { text: 'Markdown Examples', link: '/markdown-examples' },
          { text: 'Runtime API Examples', link: '/api-examples' }
        ]
      }
    ],

    socialLinks: [
      { icon: 'github', link: 'https://github.com/dmkk3r/ployd' }
    ]
  }
})
