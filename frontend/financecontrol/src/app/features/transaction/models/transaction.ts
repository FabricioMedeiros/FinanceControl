import { Category } from "./category";
import { PaymentMethod } from "./payment-method";

export interface Transaction{
    id: string;
    amount: number;
    date: Date;
    description: string;
    category : Category;
    paymentMethod: PaymentMethod;
}