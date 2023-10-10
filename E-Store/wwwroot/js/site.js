
let cartItemsCount = document.getElementById('cart-item-count');

document.addEventListener("DOMContentLoaded", function (event) {
    fetch('/ShoppingCart/GetCartItemsCount', {
        headers: { 'Content-Type': 'application/json' },
        method: 'Get',
    }).then(response => {
        if (!response.ok) {
            throw new Error(response.status);
        }

        return response.json();
    }).then(data => {
        cartItemsCount.innerHTML = data.count;

        if (data.count == 0)
            cartItemsCount.hidden = true;
        else
            cartItemsCount.hidden = false;
    }).catch(error => {
        alert('something went wrong.');
    });
});
