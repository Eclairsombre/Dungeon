#include "Animations.h"
#include <fstream>
#include <cassert>

#include "../Defines.h"

namespace Graphics {
    Animation::Animation(const std::string &file_name,
                         const std::function<void(sf::Vector2i coords, sf::Vector2i size)> &callback,
                         unsigned int timeline,
                         unsigned int offset) {
        std::ifstream file(ASSETS_DIR + "animations/" + file_name + ".txt");

        current_timeline_ = timeline;
        current_time_ = offset;

        if (!file.is_open()) {
            std::cerr << "Cannot open file " << file_name << std::endl;
            file = std::ifstream(ASSETS_DIR + "animations/animated_default.txt");
        }

        std::string line;

        int x;
        int y;
        file >> x >> y;
        file >> nb_timeline_;

        size_ = sf::Vector2i(x, y);

        std::getline(file, line);

        int i = 0;
        unsigned int nb_frame;
        int link_to_another_timeline;
        int timeline_frame_time;
        std::string txt;

        while (!file.eof() && i < nb_timeline_) {
            file >> txt;
            file >> nb_frame;
            file >> link_to_another_timeline;
            file >> timeline_frame_time;
            frames_per_timeline_.emplace_back(nb_frame);
            link_to_another_timeline_.emplace_back(link_to_another_timeline);
            timeline_frame_time_.emplace_back(timeline_frame_time);
            i++;
        }

        current_frame_ = offset / timeline_frame_time_[0];

        total_animation_time_ = timeline_frame_time_.at(current_timeline_) * frames_per_timeline_.at(current_timeline_);

        callback_ = callback;

        triggerCallback(); //To set correctly the first frame
    }

    bool Animation::paused() const {
        return paused_;
    }

    void Animation::setPaused(const bool paused) {
        paused_ = paused;
    }

    unsigned int Animation::getTimeline() const {
        return current_timeline_;
    }

    unsigned int Animation::getTimelineCount() const {
        return nb_timeline_;
    }

    void Animation::resetTimeline() {
        current_frame_ = 0;
        current_time_ = 0;
    }

    void Animation::setTimeline(const unsigned int new_timeline, bool trigger_callback) {
        assert(new_timeline < nb_timeline_); //button.txt hasn't the true number of frame

        if (new_timeline != current_timeline_ && link_to_another_timeline_.at(new_timeline) != current_timeline_) {
            resetTimeline();
            current_timeline_ = new_timeline;
            total_animation_time_ = timeline_frame_time_.at(current_timeline_) * frames_per_timeline_.at(
                                        current_timeline_);

            if (trigger_callback) {
                triggerCallback();
            }
        }
    }

    void Animation::update(const unsigned int deltaTime) {
        if (paused_ || ((frames_per_timeline_.at(current_timeline_) == 1) &&
                        (link_to_another_timeline_.at(current_timeline_) == -1))) {
            return;
        }

        current_time_ += deltaTime;

        // handle transitions
        if (link_to_another_timeline_.at(current_timeline_) != -1 && (current_time_ >= total_animation_time_)) {
            const auto offset = current_time_ % total_animation_time_;
            setTimeline(link_to_another_timeline_.at(current_timeline_), false);
            current_time_ = offset;
            current_frame_ = -1; // to force triggering callback in the if statement below
        } else {
            current_time_ %= total_animation_time_;
        }

        if (const auto new_frame = current_time_ / timeline_frame_time_.at(current_timeline_); new_frame !=
            current_frame_) {
            current_frame_ = new_frame;
            triggerCallback();
        }
    }

    void Animation::triggerCallback() const {
        callback_(sf::Vector2i(static_cast<int>(current_frame_) * size_.x,
                               static_cast<int>(current_timeline_) * size_.y),
                  size_);
    }


    sf::Vector2i Animation::getSize() const {
        return size_;
    }

    void Animation::testRegression() {
        //initialise animation
        auto animation = Animation("testRegression", [](const sf::Vector2i coords,
                                                        const sf::Vector2i size) {
        }, 2, .0);

        //test pause
        animation.setPaused(true);
        assert(animation.paused());
        animation.update(100);
        assert(animation.current_frame_ == 0);
        animation.setPaused(false);

        //test getter
        assert(animation.getTimeline() == 2);
        assert(animation.getSize() == sf::Vector2i(26, 28));

        //test animation transition

        //current timeline is 2 and the link_to_another_timeline_ of new_timeline is 0 != 2, so the timeline change
        animation.setTimeline((1));
        assert(animation.current_timeline_ == 1);

        //time between frames are set to 100, it's why we are now in the frame 1.
        animation.update(100);
        assert(animation.current_frame_ == 1);

        //resetTimeline set the frame to 0;
        animation.resetTimeline();
        assert(animation.current_frame_ == 0);

        //the update make current_time_>total_animation_time_, and link_to_another_timeline_ = 0 who isn't -1, so the timeline change to 0
        animation.update(910);
        assert(animation.current_timeline_ == 0);

        //current timeline is 0 but the link_to_another_timeline_ of new_timeline is 0, so the timeline don't change
        animation.setTimeline((1));
        assert(animation.current_timeline_ == 0);
    }
}
