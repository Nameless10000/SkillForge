import { AccessUserInfo } from "typings";

export async function getInitialState(): Promise<AccessUserInfo> {
  return { username: 'template user' };
}

export const layout = () => {
  return {
    logo: 'https://img.alicdn.com/tfs/TB1YHEpwUT1gK0jSZFhXXaAtVXa-28-27.svg',
    menu: {
      locale: false,
    },
  };
};
