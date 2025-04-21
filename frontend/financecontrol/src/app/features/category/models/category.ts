import { CategoryType } from "./category-type.enum";

export interface Category {
    id: string;
    name: string;
    description: string;
    type : CategoryType;
}