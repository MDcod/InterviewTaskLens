export function calculateMedian(arr) {
    const sortedArr = arr.slice().sort((a, b) => a - b);

    if (sortedArr.length % 2 === 0) {
        const middleIndex = sortedArr.length / 2;
        return Math.floor((sortedArr[middleIndex - 1] + sortedArr[middleIndex]) / 2);
    } else {
        const middleIndex = Math.floor(sortedArr.length / 2);
        return sortedArr[middleIndex];
    }
}