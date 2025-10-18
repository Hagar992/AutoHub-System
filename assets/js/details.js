function changeImage(src, element) {
  document.getElementById('mainimage').src = src;
  const imgs = document.querySelectorAll('.small');
  imgs.forEach(img => img.classList.remove('active'));
 element.classList.add('active');
}
window.onload = function() {
  const firstImg = document.querySelector('.small');
  firstImg.classList.add('active');
};
