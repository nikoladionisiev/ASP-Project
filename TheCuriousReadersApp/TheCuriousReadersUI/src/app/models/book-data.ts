import { AuthorModel } from "./authormodel";

export interface BookData {
    author: AuthorModel;
    id: string;
    title: string;
    quantity: number;
    description: string;
    coverUri: string;
    isAvailable: boolean;
  }
  