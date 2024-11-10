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
    public profilePicUrl: string;
    public coverPicUrl: string;
    public registeredAt: Date;

    public followersCount: number;
    public followingCount: number;
}