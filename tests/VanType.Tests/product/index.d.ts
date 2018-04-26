import { Tag } from '../tag'

export interface Product
{
	id: string;
	inStock: number;
	isVisible: boolean;
	lastUpdated: Date;
	name: string | null;
	price: number;
	status: ProductStatus;
	tags: Tag[];
}

export enum ProductStatus
{
	InStock = 0,
	OutOfStock = 1,
}

