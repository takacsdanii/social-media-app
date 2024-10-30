export class CommentModel {
    public id : number;
    public text: string;
    public createdAt: Date;
    public postId: number;
    public parentCommentId: number | null;
    public userId: string;
    public userName: string;
    public profilePicUrl: string
    public likeCount: number;
    public replyCount: number;
}