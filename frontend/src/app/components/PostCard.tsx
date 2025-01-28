import { Carousel } from "./Carousel"

type PostCardProps = {
    id?: number
    title: string
    description: string
    imageList?: string[]
}

export const PostCard: React.FC<PostCardProps> = ({
    id,
    title,
    description,
    imageList,
}) => {
    return (
        <div className="card bg-base-100 w-96 shadow-sm">
            <figure>
                {imageList && imageList.length > 0 ? (
                    <Carousel imageList={imageList} />
                ) : (
                    <img
                        src="https://img.daisyui.com/images/stock/photo-1606107557195-0e29a4b5b4aa.webp"
                        alt="Shoes"
                    />
                )}
            </figure>
            <div className="card-body">
                <h2 className="card-title">{title}</h2>
                <p>{description}</p>
                <div className="card-actions justify-end">
                    <button className="btn btn-primary">Buy Now</button>
                </div>
            </div>
        </div>
    )
}
