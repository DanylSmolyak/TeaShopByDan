export interface Product {
  id: number,
  name: string,
  description: string,
  stock: number,
  categoryID: number | null,
  rating: number | null,
  productPrices?: ProductPrice[];  // Добавлено
  photos?: Photo[];                 // Добавлено
  teaDetail?: TeaDetail;
}

export interface TeaDetail {
  id: number,
  history: string;
  preparationGuide: string;
  tastingNotes: string;
  origin: string;
  storageInstruction: string;
}

export interface Photo {
  image?: string;
}

export interface ProductPrice {
  id: number,
  price: number,
  weightGrams: number;
}
