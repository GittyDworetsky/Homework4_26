$(() => {
   
        setInterval(function () {
            updateLikes();
        }, 1000);


        $("#like-button").on('click', function () {
            const id = $("#image-id").val();
            const likeButton = $(this);
            updateLikes();
            $.post('/home/AddLike', { id }, function () {
                likeButton.prop('disabled', true);
            })
        })


        function updateLikes() {
            const id = $("#image-id").val();
            $.get(`/home/getlikes`, { id }, function ({ likes }) {
                $("#likes-count").text(likes);
            });
        
    }
})

