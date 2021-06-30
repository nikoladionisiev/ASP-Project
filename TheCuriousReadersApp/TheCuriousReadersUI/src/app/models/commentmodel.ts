export class CommentModel {
    constructor(
    public rating: number,
    public userFirstName: string,
    public userLastName: string,
    public commentBody: string
    ){}
}