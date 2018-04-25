import { Tag } from '../tag'

export interface Product
{
	id: number;
	isVisible: boolean;
	name: string;
	price: number;
	status: ProductStatus;
	tags: Tag[];
}

export enum ProductStatus
{
	InStock = 0,
	OutOfStock = 1,
}

