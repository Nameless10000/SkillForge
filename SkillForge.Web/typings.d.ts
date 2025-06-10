import '@umijs/max/typings';

export type AccessUserInfo = {
  email: string;
  id: number;
  username: string;
  role?: string;
};

export type UserLogin = {
  username: string;
  password: string;
};

export type UserReg = UserLogin & { email: string; }

export type UserTokenPayload = {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
}

export type AuthedUser = {
    data: {
        loginUser: {
            accessToken: string;
        }
    }
}
