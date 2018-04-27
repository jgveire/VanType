import { Tag } from '../tag';
import { Category } from '../tag';

export interface Product
{
	id: string;
	inStock: number;
	isVisible: boolean;
	keyWords: string[];
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

