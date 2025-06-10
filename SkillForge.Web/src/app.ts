import { UserTokenPayload } from './../typings.d';
import { AxiosResponse, RequestConfig, RequestOptions,  } from '@umijs/max';
import { AccessUserInfo, UserLogin } from "typings";
import { jwtDecode } from 'jwt-decode';

export const request: RequestConfig<UserLogin> = {
  timeout: 10_000,
  requestInterceptors: [
    (config: RequestOptions) => {

      config.method = "POST";

      return config;
    },
    (config: RequestOptions) => {

      const authToken = localStorage.getItem("token");

      if (!authToken)
        return config;  
      
      config.headers!.Authorization = `Bearer ${authToken}`

      return config;
    }
  ],
  responseInterceptors: [
    (response: AxiosResponse<any, any>) => {

      console.log("Response data:", response.data)

      return response;
    }
  ]
}

export async function getInitialState(): Promise<AccessUserInfo> {

  const authToken = localStorage.getItem("token");

  if (!authToken)
    return { username: "Unknown", id: 0, email: "" };

  const authData = jwtDecode<UserTokenPayload>(authToken);
  //console.log({authData})

  return { 
    username: authData["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"], 
    id: Number(authData["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]), 
    email: authData["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"], 
  };
}
