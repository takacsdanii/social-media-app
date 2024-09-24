import { GenderModel } from "./gender.model";

export class UserModel {
    public id: string;
    public firstName: string;
    public lastName: string;
    public userName: string;
    public email: string;
    public birthDate: Date;
    public gender: GenderModel;
}
