import { AuthorModel } from './authormodel';

export class SearchResponseModel {
    constructor(
        public id: number,
        public title: string,
        public coverUri: string, 
        public author: AuthorModel
    ){}
}