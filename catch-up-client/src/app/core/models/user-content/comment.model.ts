export class CommentModel {
    public id : number;
    public text: string;
    public createdAt: Date;
    public userId: string;
    public userName: string;
    public profilePicUrl: string
    public LikeCount: number;
    public ReplyCount: number;
}