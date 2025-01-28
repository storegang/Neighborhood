import { Carousel } from "./Carousel"

type PostCardProps = {
    id?: number
    title: string
    description: string
    images?: string[]
}

export const PostCard: React.FC<PostCardProps> = ({
    id,
    title,
    description,
    images,
}) => {
    return (
        <div className="card bg-base-100 w-96 shadow-sm">
            <figure>
                {images && images.length > 0 ? (
                    <Carousel images={images} postCardId={id} />
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
