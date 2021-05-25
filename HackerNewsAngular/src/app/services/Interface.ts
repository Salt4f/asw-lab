
export interface Submisions {
    Author: Author;
    Comments: number;
    Content: string;
    DateCreated: Date;
    Id: number;
    Title: string;
    Upvotes: string; 
}

export interface Author {
    Email: string;
    UserId: string
}

