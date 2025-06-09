import { API } from '@/constants';
import { LoginFormPage, ProFormText } from '@ant-design/pro-components';
import { request } from '@umijs/max';
import { Tabs, TabsProps } from 'antd';
import TabPane from 'antd/es/tabs/TabPane';
import { AuthedUser, UserLogin } from 'typings';

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

export default () => {
  const onFormFinish = async (data: UserLogin) => {
    const query = `query {
      loginUser (userLogin:  {
         password: "${data.password}",
         username: "${data.username}"
      }) {
        accessToken  
      }
    }`;

    //console.log(query)

    const response: AuthedUser = await request(API, {
      method: 'POST',
      data: {
        query: query,
      },
    });

    //console.log(response.data.loginUser.accessToken);
    localStorage.setItem('token', response.data.loginUser.accessToken);
  };

  return (
    <LoginFormPage<UserLogin> onFinish={onFormFinish}>
        <Tabs items={pageTabs}  defaultActiveKey='login'/>
      
    </LoginFormPage>
  );
};
