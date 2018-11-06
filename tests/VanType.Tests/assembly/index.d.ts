export interface Category
{
	id: number;
	name: string | null;
}

export interface CustomCollection
{
	capacity: number;
	count: number;
	item: CustomItem;
}

export interface CustomItem
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
	name: string | null;
}

export interface TestModel
{
	items: CustomItem[];
}

export enum ProductStatus
{
	InStock = 0,
	OutOfStock = 1,
}

