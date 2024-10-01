import { GenderModel } from "./enums/gender.model";

export class UserModel {
    public id: string;
    public firstName: string;
    public lastName: string;
    public userName: string;
    public email: string;
    public birthDate: Date;
    public gender: GenderModel;
    
    public bio: string | null;
    public profilePicUrl: string | null;
    public coverPicUrl: string | null;
    public registeredAt: Date;

    // TODO: extend model with these
    // ? MAYBE
    /* public workPlace: string;
    public schools: string[];
    public isInRelationship: boolean; */
}
