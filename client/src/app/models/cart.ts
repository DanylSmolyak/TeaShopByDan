import {cartItem} from "./cartItem";

export interface cart{
  id: number;
  userID: string;
  cartItems: cartItem [];
}
