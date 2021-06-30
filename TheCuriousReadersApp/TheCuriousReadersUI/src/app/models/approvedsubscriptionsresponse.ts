export class ApprovedSubscriptionsResponse {
    constructor(
    public id : number,
    public bookTitle: string,
    public remainingDays: number,
    public returnBookDate: string
    ){}
}