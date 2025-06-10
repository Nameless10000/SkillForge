import { API } from '@/constants';
import { LoginFormPage, ProFormText } from '@ant-design/pro-components';
import { history, request } from '@umijs/max';
import { message, Tabs, TabsProps } from 'antd';
import TabPane from 'antd/es/tabs/TabPane';
import { useState } from 'react';
import { AuthedUser, UserLogin, UserReg } from 'typings';

const pageTabs: TabsProps['items'] = [
  {
    key: "login",
    label: "Login",
    children: <>
    <ProFormText
        name={'username'}
        required
        rules={[
          {
            required: true,
            message: 'Enter username',
          },
        ]}
      />
      <ProFormText.Password
        name={'password'}
        required
        rules={[
          {
            required: true,
            message: 'Enter password',
          },
          {
            validator(_, value, callback) {
              if ((value as string).length < 6)
                callback('min password length: 6');

              callback();
            },
          },
        ]}
      />
    </>
  },
  {
    key: "reg",
    label: "Register",
    children: <>
    <ProFormText
        name={'email'}
        required
        rules={[
          {
            required: true,
            message: 'Enter email',
          },
          {
            validator(rule, value, callback) {
                if (!(value as string).includes("@"))
                  callback("Incorrect email format");

                callback();
            },
          }
        ]}
      />
    <ProFormText
        name={'username'}
        required
        rules={[
          {
            required: true,
            message: 'Enter username',
          },
        ]}
      />
      <ProFormText.Password
        name={'password'}
        required
        rules={[
          {
            required: true,
            message: 'Enter password',
          },
          {
            validator(_, value, callback) {
              if ((value as string).length < 6)
                callback('min password length: 6');

              callback();
            },
          },
        ]}
      />
    </>
  }
]

const createLoginQuery = (data: UserLogin): string => {
  return `query {
      loginUser (userLogin:  {
         password: "${data.password}",
         username: "${data.username}"
      }) {
        accessToken  
      }
    }`;
} 

const createRegQuery = (data: UserReg): string => {
  return `mutation {
      registerUser (userRegisterer:  {
         password: "${data.password}",
         username: "${data.username}",
         email: "${data.email}"
      }) {
        accessToken  
      }
    }`;
} 

export default () => {

  const [formMode, setFormMode] = useState<string>("login");

  const onFormFinish = async (data: UserReg) => {
    const query = formMode == "login" 
      ? createLoginQuery(data as UserLogin)
      : createRegQuery(data);

    const response: AuthedUser = await request(API, {
      data: {
        query,
      },
    });

    //console.log(response.data.loginUser.accessToken);
    const helloMessage = response.data.loginUser && response.data.loginUser.accessToken
      ? "Logged in!"
      : "Sth went wrong, try again :("

    if (response.data.loginUser && response.data.loginUser.accessToken) {
      localStorage.setItem('token', response.data.loginUser.accessToken);
      message.success(helloMessage);
      history.push("/profile");
    }
    else
      message.error(helloMessage);
  };

  return (
    <LoginFormPage<UserReg> onFinish={onFormFinish}>
        <Tabs items={pageTabs}  defaultActiveKey='login' onChange={setFormMode}/>
    </LoginFormPage>
  );
};
