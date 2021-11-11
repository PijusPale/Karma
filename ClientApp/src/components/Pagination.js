import React from 'react';

export const Pagination = ({ listingsPerPage, totalListings, paginate }) => {
    const pageNumbers = [];

    for (let i = 1; i <= Math.ceil(totalListings / listingsPerPage); ++i) {
        pageNumbers.push(i);
    }
    return (
        <nav>
            <ul className="pagination">
                {pageNumbers.map(number => (
                    <li key={number} className="page-item">
                        <button onClick={() => paginate(number)} className="page-link">
                            {number}
                        </button>
                    </li>
                ))}
            </ul>
        </nav>
    )
}
