// Scroll animation
const faders = document.querySelectorAll('.fade-in');
const appearOnScroll = new IntersectionObserver((entries, observer) => {
  entries.forEach(entry => {
    if(entry.isIntersecting){
      entry.target.classList.add('visible');
      observer.unobserve(entry.target);
    }
  });
}, { threshold: 0.2 });
faders.forEach(fader => appearOnScroll.observe(fader));

// Tabs
const tabs = document.querySelectorAll('.tab-btn');
const contents = document.querySelectorAll('.tab-content');

tabs.forEach(tab => {
  tab.addEventListener('click', () => {
    tabs.forEach(t => t.classList.remove('active'));
    contents.forEach(c => c.classList.remove('active'));
    tab.classList.add('active');
    document.getElementById(tab.dataset.tab).classList.add('active');
  });
});

// Change Profile Photo
const uploadInput = document.getElementById('upload-photo');
const profilePic = document.getElementById('profile-pic');

uploadInput.addEventListener('change', function(){
  const file = this.files[0];
  if(file){
    const reader = new FileReader();
    reader.onload = e => profilePic.src = e.target.result;
    reader.readAsDataURL(file);
  }
});
