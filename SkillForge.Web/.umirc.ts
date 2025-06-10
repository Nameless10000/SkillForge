import { defineConfig } from '@umijs/max';

export default defineConfig({
  antd: {},
  access: {},
  model: {},
  initialState: {},
  request: {},
  locale: {
    default: 'ru-ru',
    antd: true
  },
  proxy: {
    "/api": {
      target: "https://localhost:7090",
      pathRewrite: { "^/api": "/gql" },
      changeOrigin: true,
      secure: false
    }
  },
  layout: {
    title: 'Web-Shop',
    
  },
  routes: [
    {
      path: '/',
      redirect: '/login',
    },
    {
      path: '/login',
      component: './Login',
      layout: false
    },
    {
      path: "/profile",
      component: "./Profile",
    },
  ],
  npmClient: 'npm',
});

