export interface RegisterModel {
  username: string;
  email: string;
  password: string;
}

export interface LoginModel {
  email: string;
  password: string;
}


export interface User {
  id: string;
  username: string;
  email: string;
  image?: string;
  address?: string;
}

export interface UpdateUserProfile {
  username: string;
  address: string;
}
