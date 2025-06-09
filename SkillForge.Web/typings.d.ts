import '@umijs/max/typings';

export type AccessUserInfo = {
  username: string;
  role?: string;
};

export type UserLogin = {
  username: string;
  password: string;
};

export type AuthedUser = {
    data: {
        loginUser: {
            accessToken: string;
        }
    }
}
