"use client"

import { useState, useRef } from "react"

type CarouselProps = {
    imageList: string[]
}

export const Carousel: React.FC<CarouselProps> = ({ imageList }) => {
    const [currentIndex, setCurrentIndex] = useState(0)
    const touchStartX = useRef<number | null>(null)

    const handlePrev = () => {
        setCurrentIndex(
            (prevIndex) => (prevIndex - 1 + imageList.length) % imageList.length
        )
    }

    const handleNext = () => {
        setCurrentIndex((prevIndex) => (prevIndex + 1) % imageList.length)
    }

    const handleTouchStart = (e: React.TouchEvent) => {
        touchStartX.current = e.touches[0].clientX
    }

    const handleTouchMove = (e: React.TouchEvent) => {
        if (!touchStartX.current) return

        const touchEndX = e.touches[0].clientX
        const difference = touchStartX.current - touchEndX

        if (Math.abs(difference) > 50) {
            if (difference > 0) {
                handleNext()
            } else {
                handlePrev()
            }
            touchStartX.current = null
        }
    }

    return (
        <div
            className="relative w-full overflow-hidden"
            onTouchStart={handleTouchStart}
            onTouchMove={handleTouchMove}
        >
            <div className="relative h-124 w-full">
                {imageList.map((image, index) => (
                    <div
                        key={`slide${index}`}
                        className={`absolute inset-0 flex items-center justify-center transition-opacity duration-700 ease-in-out ${
                            index === currentIndex
                                ? "z-10 opacity-100"
                                : "z-0 opacity-0"
                        }`}
                    >
                        <img
                            src={image}
                            alt={`slide${index}`}
                            className="h-full w-full object-cover"
                        />
                    </div>
                ))}
            </div>
            {imageList.length > 1 && (
                <div className="absolute top-1/2 right-5 left-5 z-20 flex -translate-y-1/2 justify-between">
                    <button
                        onClick={handlePrev}
                        className="btn btn-circle bg-gray-800/70 p-1 text-white transition hover:bg-gray-800 md:p-2"
                    >
                        ❮
                    </button>
                    <button
                        onClick={handleNext}
                        className="btn btn-circle bg-gray-800/70 p-1 text-white transition hover:bg-gray-800 md:p-2"
                    >
                        ❯
                    </button>
                </div>
            )}
        </div>
    )
}
