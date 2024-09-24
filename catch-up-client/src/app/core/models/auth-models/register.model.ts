import { GenderModel } from "../gender.model";

export class RegisterModel {
    public firstName: string;
    public lastName: string;
    public userName: string;
    public email: string;
    public password: string;
    public passwordConfirmed: string;
    public birthDate: Date;
    public gender: GenderModel;
}