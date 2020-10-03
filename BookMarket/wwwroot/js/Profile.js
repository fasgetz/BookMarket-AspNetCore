function profileData(name) {
    $.ajax({
        type: "GET",
        url: '/Profile/AboutProfile',

        data: {
            name: name
        },

        success: function (data) {

            $(".profile__data").html(data);
        }
    });
}


function getLastVisitBooks(name) {
    $.ajax({
        type: "GET",
        url: '/Profile/LastVisitBooks',
        data: {
            name: name
        },
        success: function (data) {
            $(".profile__data").html(data);
        }
    });

}

function getCommentary(name) {
    $.ajax({
        type: "GET",
        url: '/Profile/GetCommentaries',
        data: {
            name: name
        },
        success: function (data) {

            $(".profile__data").html(data);
            var commentaries = $(".commentaryBox");
            commentaries.each(function (i, elem) {
                var rating = $(this).find("input").val();
                if (rating >= 6) {
                    $(this).addClass("commentaryBox__good");
                }
                else if (rating < 6) {
                    $(this).addClass("commentaryBox__bad");
                }

            });
        }
    });
}