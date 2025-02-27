type CarouselProps = {
    imageList: string[]
    postCardId?: number
}

export const Carousel: React.FC<CarouselProps> = ({ imageList }) => {
    return (
        <div className="carousel w-full">
            {imageList.map((image, index) => {
                return (
                    <div
                        key={`slide${index}`}
                        id={`slide${index}`}
                        className="carousel-item relative w-full"
                    >
                        <img src={image} className="w-full" />
                        <div className="absolute left-5 right-5 top-1/2 flex -translate-y-1/2 transform justify-between">
                            <a
                                href={`#slide${index - 1 < 0 ? imageList.length - 1 : index - 1}`}
                                className="btn btn-circle"
                            >
                                ❮
                            </a>
                            <a
                                href={`#slide${index + 1 > imageList.length - 1 ? 0 : index + 1}`}
                                className="btn btn-circle"
                            >
                                ❯
                            </a>
                        </div>
                    </div>
                )
            })}
        </div>
    )
}
