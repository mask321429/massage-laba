const profileData = {
    username: "Ваше имя",
    birthdate: "01.01.1990",
    profileImage: "./no-profile-min.png"
};

const usernameElement = document.getElementById("username");
const birthdateElement = document.getElementById("birthdate");
const profileImageElement = document.getElementById("profile-image");

usernameElement.textContent = profileData.username;
birthdateElement.textContent = profileData.birthdate;
profileImageElement.alt = profileData.profileImage;
profileImageElement.src = profileData.profileImage;
