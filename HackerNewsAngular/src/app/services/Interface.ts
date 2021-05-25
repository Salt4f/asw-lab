
export interface Submisions {
    Author: Author;
    Comments: number;
    Content: string;
    DateCreated: Date;
    Id: number;
    Title: string;
    Upvotes: string;
    estaVotada: boolean;
}

export interface Author {
    Email: string;
    UserId: string
}

