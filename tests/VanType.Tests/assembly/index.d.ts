export interface Category
{
	id: number;
	name: string | null;
}

export interface ProductBase
{
	id: string;
	name: string | null;
}

export interface ProductModel
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

export interface Tag
{
	id: number;
	name: string;
}

export enum ProductStatus
{
	InStock = 0,
	OutOfStock = 1,
}

