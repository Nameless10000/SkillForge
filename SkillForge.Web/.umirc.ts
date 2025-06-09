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
    title: '@umijs/max',
  },
  routes: [
    {
      path: '/',
      redirect: '/login',
    },
    {
      name: '首页',
      path: '/login',
      component: './Login',
      layout: false
    },
  ],
  npmClient: 'npm',
});

