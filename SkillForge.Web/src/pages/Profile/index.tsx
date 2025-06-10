import { PageContainer } from '@ant-design/pro-components';
import { Access, history, request, useAccess, useModel } from '@umijs/max';
import { Button, Card } from 'antd';
import Paragraph from 'antd/es/typography/Paragraph';
import Title from 'antd/es/typography/Title';
import { useEffect } from 'react';

const deleteAccountHandler = async () => {
    
    // create according method in api
    const query = `
        mutation {
            deleteUser () {
                deleted
            }
        }
    `;

    // deleteing account
    request("/api", {
        data: {
            query
        }
    })

    history.push("/")
}

export default () => {
  const { refresh, initialState } = useModel('@@initialState');
  const access = useAccess();

  return (
    <PageContainer title="Profile">
      <Card>
        <Access accessible={access.canSeeUser}>
          <Title>{initialState?.username}'s BIO</Title>
          <Paragraph>Email: {initialState?.email}</Paragraph>
          <Paragraph>Role: {initialState?.role ?? "<not set>"}</Paragraph>
          <Button danger type="primary" onClick={deleteAccountHandler}>Delete account</Button>
        </Access>
      </Card>
    </PageContainer>
  );
};
