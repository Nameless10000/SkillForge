import { AccessUserInfo } from "typings";

export default (initialState: AccessUserInfo) => {
  // 在这里按照初始化数据定义项目中的权限，统一管理
  // 参考文档 https://umijs.org/docs/max/access
  const canSeeAdmin = !!(
    initialState && initialState.role && initialState.role === "admin"
  );

  const canSeeUser = !!(initialState 
    && initialState.username 
    && initialState.username !== "Unknown");

  //console.log({canSeeUser})

  return {
    canSeeAdmin,
    canSeeUser
  };
};
